import { UserStockNote } from './user-stock-note';

describe('UserStockNote', () => {
  it('should have correct shape', () => {
    const creationTime = new Date();
    const updateTime = new Date();
    const note: UserStockNote = {
      id: 'note1',
      note: 'My analysis',
      creationTime: creationTime,
      lastUpdateTime: updateTime
    };

    expect(note.id).toBe('note1');
    expect(note.note).toBe('My analysis');
    expect(note.creationTime).toBe(creationTime);
    expect(note.lastUpdateTime).toBe(updateTime);
  });
});
